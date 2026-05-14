import '../Styles/Login.css'
import '../Styles/Global.css'

function Login () {
    return (
        <div className='container'>
            <div className='login'>
                <h1>Login</h1>
                <input type="text" placeholder='Username' />
                <input type="password" placeholder='Password' />
                <a href='/register'>Sign up</a>
                <button>Login</button>
            </div>
        </div>
    )
}

export default Login