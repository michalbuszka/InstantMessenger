import '../Styles/Login.css'
import '../Styles/Global.css'

function Register () {
    return (
        <div className='container'>
            <div className='login'>
                <h1>Register</h1>
                <input type="text" placeholder='Username' />
                <input type="password" placeholder='Password' />
                <button>Register</button>
            </div>
        </div>
    )
}

export default Register