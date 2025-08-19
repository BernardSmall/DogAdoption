using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    internal class EventManager
    {
        public delegate void NotificationDelegate(string message);
        public delegate void LogDelegate(string logEntry);

        public static event NotificationDelegate SystemNotification;
        public static event LogDelegate SystemLog;

        // Methods to trigger events
        public static void TriggerNotification(string message)
        {
            SystemNotification?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public static void TriggerLog(string logEntry)
        {
            SystemLog?.Invoke($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] LOG: {logEntry}");
        }

        public static void InitializeEventHandlers()
        {
            Dog.StatusChanged += OnDogStatusChanged;

            AdoptionApplication.ApplicationProcessed += OnApplicationProcessed;

            SystemNotification += OnSystemNotification;
            SystemLog += OnSystemLog;
        }

        private static void OnDogStatusChanged(Dog dog, string oldStatus, string newStatus)
        {
            string message = $"🐕 {dog.Name} status changed from {oldStatus} to {newStatus}";
            TriggerNotification(message);
            TriggerLog($"Dog Status Change - ID: {dog.Id}, Name: {dog.Name}, Old: {oldStatus}, New: {newStatus}");
        }

        private static void OnApplicationProcessed(AdoptionApplication application)
        {
            string message = $"📝 New adoption application: {application.Adopter.Name} applied for {application.Dog.Name}";
            TriggerNotification(message);
            TriggerLog($"Application Processed - Adopter: {application.Adopter.Name}, Dog: {application.Dog.Name}");
        }

        private static void OnSystemNotification(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[NOTIFICATION] {message}");
            Console.ForegroundColor = originalColor;
        }

        private static void OnSystemLog(string logEntry)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG] {logEntry}");

        }
    }
}
