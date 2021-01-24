using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using Tasks.Domain;
using Tasks.Infrastructure;

namespace Tasks.API
{
    public class TasksSchema : Schema
    {
        public TasksSchema(IServiceProvider provider) : base(provider)
        {
            Query = new TasksQuery();
            Mutation = new TasksMutation();
        }
    }

    public class TasksQuery : ObjectGraphType
    {
        public TasksQuery()
        {
            Field<ListGraphType<TaskItemType>>("tasks",
                resolve: context =>
                {
                    var db = context.RequestServices.GetRequiredService<TasksDbContext>();
                    return db.Tasks;
                });
        }
    }

    public class TasksMutation : ObjectGraphType
    {
        public TasksMutation()
        {
            FieldAsync<TaskItemType>("addTask",
                arguments: new QueryArguments(
                    new QueryArgument<TaskItemInputType> { Name = "task" }
                ),
                resolve: async context =>
                {
                    var task = context.GetArgument<TaskItem>("task");
                    var db = context.RequestServices.GetRequiredService<TasksDbContext>();
                    var result = await db.Tasks.AddAsync(task);
                    await db.SaveChangesAsync();
                    return result.Entity;
                });
        }
    }

    public class TaskItemType : ObjectGraphType<TaskItem>
    {
        public TaskItemType()
        {
            Field(o => o.Id);
            Field(o => o.Title);
            Field(o => o.DueDate, type: typeof(DateGraphType), nullable: true);
            Field(o => o.Description, nullable: true);
        }
    }

    public class TaskItemInputType : InputObjectGraphType
    {
        public TaskItemInputType()
        {
            Name = "TaskItemInput";
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<DateGraphType>("dueDate");
            Field<StringGraphType>("description");
        }
    }
}
