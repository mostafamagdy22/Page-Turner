using PageTurner.Models;

namespace PageTurner.Services.Interfaces
{
	public interface IEmailSender
	{
		public void SendEmail(Message message);
	}
}
