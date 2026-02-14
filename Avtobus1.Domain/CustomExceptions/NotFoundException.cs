namespace Avtobus1.Domain.CustomExceptions;

public class NotFoundException(string message) : Exception(message);
