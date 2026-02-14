namespace Avtobus1.Domain.CustomExceptions;

public class AlreadyExistsException(string message) : Exception(message);
