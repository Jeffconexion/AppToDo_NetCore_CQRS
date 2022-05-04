﻿using System.Threading.Tasks;
using AppTodo.Application.Commands.Contracts;
using AppTodo.Application.Commands.Handlers.Contracts;
using AppTodo.Core.Entities;
using AppTodo.Core.IRepositories;
using Flunt.Notifications;

namespace AppTodo.Application.Commands.Handlers
{
  /// <summary>
  /// @author: Jefferson Santos
  /// @Data  : 04/05/2022
  ///
  /// Class to execution commands.
  /// </summary>
  public class TodoHandler : Notifiable, IHandler<CreateTodoCommand>, IHandler<UpdateTodoCommand>, IHandler<MarkTodoAsDoneCommand>, IHandler<MarkTodoAsUndoneCommand>
  {
    private readonly ITodoRepository _repository;

    public TodoHandler(ITodoRepository repository)
    {
      _repository = repository;
    }

    /// <summary>
    /// Execution command Create task.
    /// </summary>
    /// <param name="command">object command.</param>
    /// <returns></returns>
    public async Task<ICommandResult> Handle(CreateTodoCommand command)
    {
      command.Validate();

      if (command.Invalid)
        return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada!", command.Notifications);

      TodoItem todo = new TodoItem(command.Title, command.User, command.Date);

      await _repository.Create(todo);

      return new GenericCommandResult(true, "Tarefa Salva", todo);
    }

    /// <summary>
    /// Execution command Update task.
    /// </summary>
    /// <param name="command">Object command.</param>
    /// <returns></returns>
    public async Task<ICommandResult> Handle(UpdateTodoCommand command)
    {
      command.Validate();
      if(command.Invalid)
        return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada!", command.Notifications);

      //recover the todoItem
      TodoItem todo = await _repository.GetById(command.Id, command.User);

      //change title
      todo.UpdateTitle(command.Title);

      //save in database
      await _repository.Update(todo);

      return new GenericCommandResult(true, "Tarefa Salva", todo);
    }

    /// <summary>
    /// Execution command Mark task as done.
    /// </summary>
    /// <param name="command">Object command.</param>
    /// <returns></returns>
    public async Task<ICommandResult> Handle(MarkTodoAsDoneCommand command)
    {
      command.Validate();
      if (command.Invalid)
        return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada!", command.Notifications);

      //recover the todoItem
      TodoItem todo = await _repository.GetById(command.Id, command.User);

      //change title
      todo.MarkAsDone();

      //save in database
      await _repository.Update(todo);

      return new GenericCommandResult(true, "Tarefa Salva", todo);
    }

    /// <summary>
    /// Execution command Mark task as undone.
    /// </summary>
    /// <param name="command">object command.</param>
    /// <returns></returns>
    public async Task<ICommandResult> Handle(MarkTodoAsUndoneCommand command)
    {
      command.Validate();
      if (command.Invalid)
        return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada!", command.Notifications);

      //recover the todoItem
      TodoItem todo = await _repository.GetById(command.Id, command.User);

      //change title
      todo.MarkAsUndone();

      //save in database
      await _repository.Update(todo);

      return new GenericCommandResult(true, "Tarefa Salva", todo);
    }
  }
}
