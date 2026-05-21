import '../Styles/Message.css'

interface messageProps 
{
    sender: string,
    content: string,
    messageClass: string
}

function Message (props: messageProps) {
    const getMessageAlign = (messageClass: string) => {
        if (messageClass == "myMessage")
            return "flex-start";
        return "flex-end"
    }
    return (
    <div className="message" style={{alignSelf: getMessageAlign(props.messageClass)}}>
        <p className="messageAuthor">{props.sender}</p>
        <span className={"messageContent" + " " + props.messageClass}>{props.content}</span>
    </div>)
}

export default Message;