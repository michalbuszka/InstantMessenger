import '../Styles/Global.css'
import '../Styles/Conversation.css'
import Message from '../components/Message.tsx';

function Conversation () {
    return(           
        <div className="conversation">
            <div className="messages">
                <Message sender='Michał Buszka' content='siema' messageClass='myMessage' />
                <Message sender='Michał Buszka' content='siema' messageClass='theirMessage' />
                <Message sender='Michał Buszka' content='sidqwdqwd qwd qwdqwdqwdqwdqwd q dqwdqwdqwdqwdqwdqwdqwdw dqwema' messageClass='myMessage' />
            </div>
            <div className="messageType">
                <input type="text" placeholder="Type a message..." />
                <button>Send</button>
            </div>
        </div>
    )
}

export default Conversation;