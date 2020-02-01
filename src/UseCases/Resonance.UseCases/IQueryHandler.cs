﻿using Resonance.UseCases.Contracts.Requests;
using Resonance.UseCases.Contracts.Responses;

namespace Resonance.UseCases
{
    /// <summary>
    /// Handler responsible for handling given <see cref="TQuery"/>
    /// </summary>
    /// <typeparam name="TQuery">Query (retrieve something) to be handled</typeparam>
    /// <typeparam name="TResponse">Response on handling query</typeparam>
    public interface IQueryHandler<TQuery, TResponse> : IQueryHandler
        where TQuery : IQuery where TResponse : IResponse<TQuery>
    {
        /// <summary>
        /// Handle query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Result with corresponding <see cref="TResponse"/></returns>
        OperationResult<TQuery, TResponse> Handle(TQuery query);
    }

    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IQueryHandler
    {
    }
}
