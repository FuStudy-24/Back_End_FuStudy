using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Quartz;

[DisallowConcurrentExecution]
public class UpdateConversationIsCloseJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConversationIsCloseJob(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            var currentTime = DateTime.Now;

            var bookings = _unitOfWork.BookingRepository
                .Get(b => b.Status == "Pending");

            foreach (var booking in bookings)
            {
                var overTimeBooking = booking.CreateAt.AddHours(1);
                if (overTimeBooking <= currentTime)
                {
                    booking.Status = "OverTime";
                    _unitOfWork.BookingRepository.Update(booking);
                }

            }

            //Auto End Booking & close conversation
            var acceptBookings = _unitOfWork.BookingRepository
                .Get(b => b.Status == "Accepted")
                .ToList();

            foreach (var booking in acceptBookings)
            {
                if (booking.EndTime <= currentTime)
                {
                    // Fetch the corresponding conversation
                    var conversation = _unitOfWork.ConversationRepository.Get(
                        c => c.User1Id == booking.UserId &&
                        c.User2Id == booking.MentorId &&
                        c.EndTime == booking.EndTime &&
                        c.IsClose == false).FirstOrDefault();

                    if (conversation != null)
                    {
                        booking.Status = "Ended";
                        conversation.IsClose = true;

                        _unitOfWork.BookingRepository.Update(booking);
                        _unitOfWork.ConversationRepository.Update(conversation);
                    }
                }
            }

            /*var conversations = _unitOfWork.ConversationRepository
                .Get(c => c.IsClose == false)
                .ToList();

            foreach (var conversation in conversations)
            {
                var endTimeWithDuration = conversation.EndTime;
                if (endTimeWithDuration <= currentTime)
                {
                    conversation.IsClose = true;
                    _unitOfWork.ConversationRepository.Update(conversation);
                }
            }*/

            //Auto openconversation
            var openConversations = _unitOfWork.ConversationRepository
                .Get(oc => oc.IsClose == true)
                .ToList();

            foreach (var conversation in openConversations)
            {
                if (conversation.CreateAt >= currentTime)
                {
                    var startTimeWithDuration = conversation.EndTime - conversation.Duration;
                    var booking = _unitOfWork.BookingRepository.Get(
                            c => c.UserId == conversation.User1Id &&
                            c.MentorId == conversation.User2Id &&
                            c.EndTime == conversation.EndTime &&
                            c.Status == "Accepted").FirstOrDefault();
                    if (booking != null)
                    {
                        conversation.IsClose = false;
                        _unitOfWork.ConversationRepository.Update(conversation);
                    }
                }
                    
            }

            var studentSubcriptions = _unitOfWork.StudentSubcriptionRepository
                .Get(c => c.Status == true)
                .ToList();

            foreach(var studentSubcription in studentSubcriptions)
            {
                var endTimeWithDuration = studentSubcription.EndDate;
                if(endTimeWithDuration <= currentTime)
                {
                    studentSubcription.Status = false;
                    _unitOfWork.StudentSubcriptionRepository.Update(studentSubcription);
                }
            }

            

            _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return Task.CompletedTask;
    }

}
