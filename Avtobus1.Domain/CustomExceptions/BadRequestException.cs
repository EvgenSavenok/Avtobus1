namespace Avtobus1.Domain.CustomExceptions;

public class BadRequestException(string message) : Exception(message);
