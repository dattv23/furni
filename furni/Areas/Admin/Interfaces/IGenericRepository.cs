using System.Linq.Expressions;

namespace furni.Interfaces
{
    /// <summary>
    /// Defines a generic repository interface for managing entities of type <typeparamref name="T"/>.
    /// This interface abstracts the CRUD operations for any entity type, promoting a clean separation
    /// of concerns and code reusability within the eCommerce API.
    /// </summary>
    /// <typeparam name="T">The entity type this repository will manage. Must be a class.</typeparam>
    public interface IGenericRepository<T, TKey> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves all instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all instances of <typeparamref name="T"/>.</returns>
        Task<List<T>> GetAllAsync();


        /// <summary>
        /// Asynchronously retrieves a specific instance of <typeparamref name="T"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the instance of <typeparamref name="T"/> if found; otherwise, null.</returns>
        Task<T> GetByIdAsync(TKey id);

        /// <summary>
        /// Asynchronously retrieves a specific instance of <typeparamref name="T"/> by its identifier, including related entities as specified by the implementation.
        /// </summary>
        /// <param name="id">The unique identifier for the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the instance of <typeparamref name="T"/>, including related entities, if found; otherwise, null.</returns>
        Task<T> GetByIdWithIncludesAsync(TKey id);

        /// <summary>
        /// Asynchronously removes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to be removed.</param>
        /// <returns>A task that represents the asynchronous remove operation, without returning any result.</returns>
        /// <remarks>
        /// This method should find the entity by the given identifier and remove it from the context, followed by an asynchronous save operation to update the database.
        /// Implementations need to handle cases where the entity with the specified ID does not exist, potentially by throwing a custom exception or handling it gracefully.
        /// </remarks>
        Task RemoveAsync(TKey id);

        /// <summary>
        /// Asynchronously adds a new entity of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">The entity to add. This is passed by reference using the 'in' keyword to avoid unnecessary copying while still preventing modifications.</param>
        /// <returns>A task that represents the asynchronous add operation. The task result contains the number of state entries written to the underlying database as an int.</returns>
        /// <remarks>
        /// This method is responsible for adding the entity to the context and then saving the changes asynchronously to the database.
        /// Implementations should ensure that the entity is validated before being added to ensure it meets any defined constraints or rules.
        /// </remarks>
        Task<int> AddAsync(T entity);


        /// <summary>
        /// Asynchronously updates the specified entity of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">The entity to update. This is passed by reference using the 'in' keyword to avoid unnecessary copying while still preventing modifications.</param>
        /// <returns>A task that represents the asynchronous update operation, without returning any result.</returns>
        /// <remarks>
        /// This method should ensure concurrency handling, and apply any changes made to the entity in the context to the underlying database.
        /// Implementations should also handle exceptions that might occur during the database update operation, such as concurrency conflicts or validation errors.
        /// </remarks>
        Task UpdateAsync(TKey id, T entity);


        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the underlying database.</returns>
        Task<int> SaveAsync();


        /// <summary>
        /// Asynchronously selects a single instance of <typeparamref name="T"/> based on a specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an instance of <typeparamref name="T"/> that satisfies the condition specified by <paramref name="predicate"/>.</returns>
        Task<T> SelectAsync(Expression<Func<T, bool>> predicate);
    }
}
