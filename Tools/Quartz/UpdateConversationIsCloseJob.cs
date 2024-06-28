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
                var overTimeBooking = booking.CreateAt.AddMinutes(5);
                if (overTimeBooking <= currentTime)
                {
                    booking.Status = "OverTime";
                    _unitOfWork.BookingRepository.Update(booking);
                }

            }

            var acceptBookings = _unitOfWork.BookingRepository
                .Get(b => b.Status == "Accept");

            foreach (var booking in acceptBookings)
            {
                if (booking.EndTime >= currentTime)
                {
                    booking.Status = "End";
                    //var conversation = _unitOfWork.ConversationRepository.Get

                    _unitOfWork.BookingRepository.Update(booking);
                }
            }

            var conversations = _unitOfWork.ConversationRepository
                .Get(c => c.IsClose == false)
                .ToList();
            var bk =  _unitOfWork.BookingRepository.Get(b => b.Status == "End").FirstOrDefault();

            foreach (var conversation in conversations)
            {
                    var endTimeWithDuration = conversation.EndTime;
                    if (endTimeWithDuration <= currentTime)
                    {
                        conversation.IsClose = true;
                        _unitOfWork.ConversationRepository.Update(conversation);
                    }
            }

            var openconversations = _unitOfWork.ConversationRepository
                .Get(oc => oc.IsClose == true)
                .ToList() ;

            foreach (var conversation in openconversations)
            {
                var startTimeWithDuration = conversation.EndTime - conversation.Duration;
                if (startTimeWithDuration == currentTime)
                {
                    conversation.IsClose = false;
                    _unitOfWork.ConversationRepository.Update(conversation);
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
