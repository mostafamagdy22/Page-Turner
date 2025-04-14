using AutoMapper;
using PageTurner.Models;
using PageTurner.Models.ViewModels;

namespace PageTurner.Services.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<AddBookViewModel, Book>()
	.ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
	.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
	.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
	.ForMember(dest => dest.Descreption, opt => opt.MapFrom(src => src.Descreption))
	.ForMember(dest => dest.Pages, opt => opt.MapFrom(src => src.Pages))
	.ForMember(dest => dest.Formula, opt => opt.MapFrom(src => src.Formula))
	.ForMember(dest => dest.Image, opt => opt.Ignore())
	.ForMember(dest => dest.PublisherID, opt => opt.MapFrom(src =>
	src.SelectedPublisher))
	.ForMember(dest => dest.BookAuthors, opt => opt.MapFrom(src =>
	src.SelectedAuthors.Select(authorId => new BookAuthor { AuthorID = authorId })))
	.ForMember(dest => dest.BookCategories, opt => opt.MapFrom(src =>
	src.SelectedCategories.Select(categoryId => new BookCategories { CategoryID = categoryId }))).ReverseMap();


			CreateMap<Book, BookCardViewModel>();

			CreateMap<Author, SelectableAuthorViewModel>().ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Name))
				.ReverseMap();

			CreateMap<Author, AddAuthorViewModel>().ReverseMap();

			CreateMap<Publisher, SelectablePublisherViewModel>()
				.ForMember(dest => dest.PublisherID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Name))
				.ReverseMap();

			CreateMap<Review,CreateReviewViewModel>().ReverseMap();

			CreateMap<Review, ReviewDetailsViewModel>()
				.ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.book.Title))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.user.UserName))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ReverseMap();
		}
	}
}
