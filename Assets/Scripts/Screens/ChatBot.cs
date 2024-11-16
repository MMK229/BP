using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBot : MonoBehaviour
{
    public GameObject chatPanel;
    public GameObject messageText;

    List<ChatBotMessage> messageList = new List<ChatBotMessage>();


    public void SendMessageToChatPanel(ZMVideoSDKChatMessage messageContent)
    {
        ChatBotMessage chatBotMessage = new ChatBotMessage();
        GameObject newMessageText = Instantiate(messageText, chatPanel.transform);
        chatBotMessage.messageContent = messageContent;
        chatBotMessage.messageText = newMessageText.GetComponent<TMP_Text>();
        string mesageContent = messageContent.senderUser.GetUserName() + ": " + messageContent.content;
        chatBotMessage.messageText.text = mesageContent;
        messageList.Add(chatBotMessage);
    }

    public void DeleteMessageInChatPanel(string msgID)
    {
        foreach (ChatBotMessage message in messageList)
        {
            if (string.Equals(msgID, message.messageContent.msgId))
            {
                messageList.Remove(message);
                return;
            }
        }
    }

    public void ClearChatPanel()
    {
        foreach (ChatBotMessage message in messageList)
        {
            DestroyGameObjectImmediate(message.messageText);
        }
        messageList.Clear();
    }

    private void DestroyGameObjectImmediate(UnityEngine.Object gameObject)
    {
        if (gameObject != null)
        {
            DestroyImmediate(gameObject);
        }
    }

    [SerializeField]
    public struct ChatBotMessage
    {
        public TMP_Text messageText;

        public ZMVideoSDKChatMessage messageContent;
    }
}
