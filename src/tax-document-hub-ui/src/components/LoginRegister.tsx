import React, {useState} from 'react';
import { useNavigate } from 'react-router-dom';


export default function LoginRegister(){
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoginMode, setIsLoginMode] = useState(true);
    const [error] = useState<string | null>(null);

    const navigate = useNavigate();


    const handleSubmit = async (e: React.FormEvent)=>{
        e.preventDefault();
        console.log("Form submitted")

        var url = `http://localhost:5032/api/auth/${isLoginMode? 'login': 'register'}`

        var response = await fetch(url, {
            method: 'POST',
            headers:{
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email,
                password
            })
        })

        if(!response.ok){
            console.log("something went wrong")
            console.log(response.text())
        }

       const data = await response.json()
       const token = data.token;  //email userId

       console.log(token)
       localStorage.setItem('token', token);

       console.log(data)
       navigate('/dashboard')

    }

    
    return(
        <div className="auth-wrapper">
            <div className="auth-card">
                <h1 className="auth-title">{isLoginMode? 'Login' : 'Register'}</h1>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label className="form-label" htmlFor="email">Email</label>
                        <input className="form-input" type="email" id="email"
                        value={email}
                        onChange={(e)=> setEmail(e.target.value)}/>
                    </div>

                    <div className="form-group">
                        <label className="form-label" htmlFor="password">Passwort</label>
                        <input className="form-input" type='password' id="password"
                        value={password}
                        onChange={(e)=> setPassword(e.target.value)}/>
                    </div>
                    { error && <div className="alert alert-error">{error}</div> }
                    <button type="submit" className="btn btn-primary btn-block">
                        {isLoginMode? 'Anmelden' : 'Registrieren'}
                    </button>
                    <a className="toggle-link" onClick={()=> setIsLoginMode(!isLoginMode)}>
                        {isLoginMode? 'Noch kein Account?' : 'Sie haben bereits einen Account?'}
                    </a>
                </form>
            </div>
        </div>
    )
}