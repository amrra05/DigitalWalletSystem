import { useState } from "react";
import { Link } from "react-router-dom";
import { forgotPassword } from "../services/authService";
import "./styles/ForgotPassword.css";


function ForgotPassword()
{

    const [email,setEmail] = useState("");

    const [message,setMessage] = useState("");

    const [loading,setLoading] = useState(false);



    async function handleSubmit(e)
    {

        e.preventDefault();


        try
        {

            setLoading(true);


            const response =
                await forgotPassword({
                    email
                });


            setMessage(
                response.message
            );


        }
        catch(error)
        {

            console.log(error);


            setMessage(
                error.response?.data?.message
                ||
                "Something went wrong"
            );

        }
        finally
        {

            setLoading(false);

        }


    }





    return (

        <div className="forgot-page">


            <div className="forgot-card">


                <h1>
                    Forgot Password
                </h1>


                <p>
                    Enter your email to receive a reset link
                </p>



                <form onSubmit={handleSubmit}>


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



                    <button
                    disabled={loading}
                    >

                        {
                            loading
                            ?
                            "Sending..."
                            :
                            "Send Reset Link"
                        }

                    </button>



                </form>




                {
                    message &&

                    <p className="message">
                        {message}
                    </p>
                }



                <Link to="/login">

                    Back to Login

                </Link>



            </div>


        </div>

    );

}


export default ForgotPassword;