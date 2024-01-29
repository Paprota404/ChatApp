import {useState} from 'react';
import {supabase} from '../supabaseClient';

async function signUp(email:string, password:string) {
    const { user, session, error } = await supabase.auth.signUp({
       email: email,
       password: password,
    });
   }

export default function SignUpPage(){
    const [email,setEmail] = useState('');
    const [password,setPassword] = useState('');

    const handleEmailChange = (event) =>{
        setEmail(event.target.value);
    }
    const handlePasswordChange = (event) =>{
        setPassword(event.target.value);
    }
    const handleSubmit = (event) =>{
        event.preventDefault();
        signUp(email,password)
    }

    return (
        <div>
          <h2>Sign Up Form</h2>
          <form onSubmit={handleSubmit}>
            <div>
              <label>Email:</label>
              <input
                type="email"
                value={email}
                onChange={handleEmailChange}
                required            
              />
            </div>
            <div>
              <label>Password:</label>
              <input
                type="password"
                value={password}
                onChange={handlePasswordChange}
                required
              />
            </div>
            <div>
              <button type="submit">Sign Up</button>
            </div>
          </form>
        </div>
     );
    
}