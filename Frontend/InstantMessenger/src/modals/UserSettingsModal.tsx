import ReactDOM from 'react-dom'
import '../Styles/UserSettingsModal.css'

function UserSettingsModal () {
    return ReactDOM.createPortal(
    <div className='userSettingsModal'>
        <input type='text' />
    </div>, document.body)
}

export default UserSettingsModal;