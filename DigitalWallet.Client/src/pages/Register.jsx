import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../services/authService";
import PasswordInput from "../components/pass/PasswordInput";
import PasswordStrength from "../components/pass/PasswordStrength";
import "../components/pass/Password.css"
import "./styles/Register.css";


function Register()
{

    const navigate = useNavigate();



    const [name,setName] = useState("");

    const [email,setEmail] = useState("");

    const [password,setPassword] = useState("");

    const [confirmPassword,setConfirmPassword] = useState("");





    const passwordsMatch =
        password === confirmPassword &&
        confirmPassword.length > 0;





    const passwordRules =
    {

        length:
            password.length >= 8,


        uppercase:
            /[A-Z]/.test(password),


        lowercase:
            /[a-z]/.test(password),


        number:
            /\d/.test(password),


        special:
            /[^A-Za-z0-9]/.test(password)

    };




    const isPasswordValid =
        Object.values(passwordRules)
        .every(Boolean);






    async function handleRegister(e)
    {

        e.preventDefault();



        if(!isPasswordValid)
        {

            alert(
                "Password does not meet requirements"
            );

            return;

        }



        if(!passwordsMatch)
        {

            alert(
                "Passwords do not match"
            );

            return;

        }




        try
        {


            const response =
                await register({

                    name,

                    email,

                    password

                });



            console.log(response);



            alert(
            "Registration successful!\n\nPlease verify your email before logging in."
            );

            navigate("/login");


        }
        catch(error)
        {


            console.log(error);


            alert(
                error.response?.data?.message
                ||
                "Registration failed"
            );


        }


    }





    return (

        <div className="register-page">


            <div className="register-card">


                <h1>
                    Create Account
                </h1>



                <p className="subtitle">

                    Join Digital Wallet

                </p>





                <form onSubmit={handleRegister}>




                    <div className="input-group">


                        <label>
                            Name
                        </label>



                        <input

                        type="text"

                        placeholder="Enter your name"

                        value={name}

                        onChange={(e)=>
                            setName(
                                e.target.value
                            )
                        }

                        />


                    </div>






                    <div className="input-group">


                        <label>
                            Email
                        </label>



                        <input

                        type="email"

                        placeholder="Enter your email"

                        value={email}

                        onChange={(e)=>
                            setEmail(
                                e.target.value
                            )
                        }

                        />


                    </div>









                    <div className="input-group">


                        <label>
                            Password
                        </label>



                        <PasswordInput

                        value={password}

                        setValue={setPassword}

                        placeholder="Enter password"

                        />



                        <PasswordStrength

                        password={password}

                        />





                        <div className="password-rules">


                            <p className={
                                passwordRules.length
                                ?
                                "valid"
                                :
                                "invalid"
                            }>

                                {
                                passwordRules.length
                                ?
                                "✔"
                                :
                                "✖"
                                }

                                {" "}
                                At least 8 characters

                            </p>





                            <p className={
                                passwordRules.uppercase
                                ?
                                "valid"
                                :
                                "invalid"
                            }>

                                {
                                passwordRules.uppercase
                                ?
                                "✔"
                                :
                                "✖"
                                }

                                {" "}
                                One uppercase letter

                            </p>






                            <p className={
                                passwordRules.lowercase
                                ?
                                "valid"
                                :
                                "invalid"
                            }>

                                {
                                passwordRules.lowercase
                                ?
                                "✔"
                                :
                                "✖"
                                }

                                {" "}
                                One lowercase letter

                            </p>







                            <p className={
                                passwordRules.number
                                ?
                                "valid"
                                :
                                "invalid"
                            }>

                                {
                                passwordRules.number
                                ?
                                "✔"
                                :
                                "✖"
                                }

                                {" "}
                                One number

                            </p>







                            <p className={
                                passwordRules.special
                                ?
                                "valid"
                                :
                                "invalid"
                            }>

                                {
                                passwordRules.special
                                ?
                                "✔"
                                :
                                "✖"
                                }

                                {" "}
                                One special character

                            </p>


                        </div>



                    </div>









                    <div className="input-group">


                        <label>
                            Confirm Password
                        </label>



                        <PasswordInput

                        value={confirmPassword}

                        setValue={setConfirmPassword}

                        placeholder="Confirm password"

                        />





                        {
                        confirmPassword.length > 0 &&



                        <p

                        className={
                            passwordsMatch
                            ?
                            "valid"
                            :
                            "invalid"
                        }

                        >

                        {
                            passwordsMatch
                            ?
                            "✔ Passwords match"
                            :
                            "✖ Passwords do not match"
                        }


                        </p>

                        }



                    </div>









                    <button

                    className="register-btn"

                    type="submit"

                    disabled={
                        !passwordsMatch ||
                        !isPasswordValid
                    }

                    >

                        Register


                    </button>





                </form>








                <p className="login-text">


                    Already have an account?



                    <Link

                    to="/login"

                    className="login-link"

                    >

                        Login


                    </Link>


                </p>





            </div>


        </div>

    );

}



export default Register;