using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ADFirstApp.Services
{
    public class AzQueueService
    {
        private readonly QueueClient _queueClient;
        public AzQueueService(string queueName)
        {
            _queueClient = new QueueClient(ConnectionStrings.AzureStorageConnectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                await _queueClient.SendMessageAsync(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                var queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));

                var messages = queueMessages.Value;
                if (messages != null)
                {
                    if (messages.Any())
                    {
                        return messages[0];
                    }
                }
            }
            return null!;
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }


        public async Task<List<QueueMessage>> GetAllMessagesFromQueueAsync()
        {

            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                var queueMessages = await _queueClient.ReceiveMessagesAsync(properties.ApproximateMessagesCount, TimeSpan.FromSeconds(30));

                var messages = queueMessages.Value;
                if (messages != null)
                {
                    if (messages.Any())
                    {
                        return messages.ToList();
                    }
                }
            }
            return null!;
        }
    }
}

