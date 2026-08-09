import { useState } from "react";
import { login } from "../services/authService";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../context/AuthContext";
import { Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "./styles/Login.css";


function Login() {

    const navigate = useNavigate();

    const { loginUser } = useContext(AuthContext);

    const [email, setEmail] = useState("");

    const [password, setPassword] = useState("");

    const [error, setError] = useState("");

    const [showPassword, setShowPassword] = useState(false);



    async function handleSubmit(e)
    {

        e.preventDefault();


        setError("");


        try
        {

            const response =
                await login({
                    email,
                    password
                });

                           const accessToken =
    response.token.accessToken;


            await loginUser(accessToken);


                localStorage.setItem(
                    "refreshToken",
                    response.token.refreshToken
                );



                const decoded =
                    jwtDecode(accessToken);

                    const role =
                        decoded[
                            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                        ];
                    console.log(decoded);
                    console.log("Role:", role);
                    console.log(
    role === "Admin"
);
                if(role === "Admin")
                {
                    navigate("/Admin");
                }
                else
                {
                    navigate("/dashboard");
                }


        }
        catch(error)
        {

            setError(
                "Invalid email or password"
            );

        }

    }



    return (

        <div className="login-page">

            <div className="login-card">


                <h1>
                    Welcome
                </h1>


                <p className="subtitle">
                    Sign in to your Digital Wallet
                </p>



                <form onSubmit={handleSubmit}>


                    <div className="input-group">

                        <label>
                            Email
                        </label>


                        <input

                            type="email"

                            placeholder="Enter your email"

                            value={email}

                            onChange={
                                (e)=>
                                setEmail(e.target.value)
                            }

                        />


                    </div>





                    <div className="input-group">


                        <label>
                            Password
                        </label>



                        <div className="password-container">


                            <input

                                type={
                                    showPassword
                                    ? "text"
                                    : "password"
                                }

                                placeholder="Enter your password"


                                value={password}


                                onChange={
                                    (e)=>
                                    setPassword(e.target.value)
                                }


                            />



                            <button

                                type="button"

                                className="show-password-btn"


                                onClick={
                                    () =>
                                    setShowPassword(
                                        !showPassword
                                    )
                                }

                            >

                                {
                                    showPassword
                                    ?
                                    "Hide"
                                    :
                                    "Show"
                                }


                            </button>



                        </div>


                    </div>



                    {
                        error &&

                        <p className="error-message">

                            {error}

                        </p>
                    }

                        <Link 
                            to="/forgot-password"
                            className="forgot-link"
                        >
                            Forgot Password?
                        </Link>


                    <button

                        className="login-btn"

                        type="submit"

                    >

                        Login

                    </button>



                </form>




                <p className="register-text">

                    Don't have an account?


                    <Link
                        to="/register"
                        className="register-link"
                    >
                        Register
                    </Link>


                </p>




            </div>


        </div>

    );

}


export default Login;