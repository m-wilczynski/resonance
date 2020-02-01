using Resonance.UseCases.Contracts.Requests;
using Resonance.UseCases.Contracts.Responses;

namespace Resonance.UseCases
{
    /// <summary>
    /// Handler responsible for give <see cref="TCommand"/>
    /// </summary>
    /// <typeparam name="TCommand">Command (to modify system) to be handled</typeparam>
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        /// <summary>
        /// Handle command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>Result of handling command</returns>
        OperationResult Handle(TCommand command);
    }

    /// <summary>
    /// Handler responsible for give <see cref="TCommand"/>
    /// </summary>
    /// <typeparam name="TCommand">Command (to modify system) to be handled</typeparam>
    /// <typeparam name="TResponse">Response on handling command</typeparam>
    public interface ICommandHandler<TCommand, TResponse> : ICommandHandler
        where TCommand : ICommand where TResponse : IResponse<TCommand>
    {
        /// <summary>
        /// Handle command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>Result with corresponding <see cref="TResponse"/></returns>
        OperationResult<TCommand, TResponse> Handle(TCommand command);
    }


    /// <summary>
    /// Marker interface
    /// </summary>
    public interface ICommandHandler
    {
    }
}
