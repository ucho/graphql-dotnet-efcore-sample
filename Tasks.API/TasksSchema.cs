using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using System.Linq;
using Tasks.Domain;
using Tasks.Infrastructure;

namespace Tasks.API
{
    public class TasksSchema : Schema
    {
        public TasksSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<TasksQuery>();
            Mutation = provider.GetRequiredService<TasksMutation>();
        }
    }

    public class TasksQuery : ObjectGraphType
    {
        public TasksQuery(TasksDbContext db)
        {
            Field<ListGraphType<TaskItemType>>("tasks",
                resolve: _ => db.Tasks.AsEnumerable());
        }
    }

    public class TasksMutation : ObjectGraphType
    {
        public TasksMutation(TasksDbContext db)
        {
            Field<TaskItemType>("addTask",
                arguments: new QueryArguments(
                    new QueryArgument<TaskItemInputType> { Name = "task" }
                ),
                resolve: context =>
                {
                    var task = context.GetArgument<TaskItem>("task");
                    var result = db.Tasks.Add(task);
                    db.SaveChanges();
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
