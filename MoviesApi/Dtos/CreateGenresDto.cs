namespace MoviesApi.Dtos
{
    public class CreateGenresDto
    {
        [MaxLength(length:100)]
        public string Name { get; set; } = null!;
    }
}
