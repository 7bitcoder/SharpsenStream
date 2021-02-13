using SharpsenStreamBackend.Classes;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.StreamChat
{
    public class ChatRoom
    {
        int chatId;
        ConcurrentDictionary<ChatMember, ChatMember> _members = new ConcurrentDictionary<ChatMember, ChatMember>();
        BlockingCollection<Message> _chatMessages = new BlockingCollection<Message>();
        public ChatRoom(int id)
        {
            chatId = id;
            run();
        }

        public async Task handleUser(ChatMember user)
        {
            AddMember(user);
            await ReadUserMessages(user);
            RemoveMember(user);
        }

        private void AddMember(ChatMember member)
        {
            _members.TryAdd(member, member);
        }

        private void RemoveMember(ChatMember member)
        {
            _members.TryRemove(member, out ChatMember outValue);
        }
        private void run()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var message = _chatMessages.Take();
                    foreach (var member in _members)
                    {
                        member.Value.sendMessage(message);
                    }
                }
            });
        }

        private async Task ReadUserMessages(ChatMember member)
        {
            try
            {
                while (true)
                {
                    var message = await member.getMessage();
                    _chatMessages.Add(message);
                }
            }
            catch (UserDisconnected e)
            {

            }
            finally
            {
                //just finish
            }
        }
    }
}
