import { useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { resetPassword } from "../services/authService";
import PasswordInput from "../components/pass/PasswordInput";
import PasswordStrength from "../components/pass/PasswordStrength";
import "../components/pass/Password.css";
import "./styles/ResetPassword.css";


function ResetPassword()
{

    const [searchParams] = useSearchParams();


    const token =
        searchParams.get("token");



    const [password,setPassword] = useState("");

    const [confirmPassword,setConfirmPassword] = useState("");



    const [message,setMessage] = useState("");

    const [loading,setLoading] = useState(false);



    async function handleSubmit(e)
    {

        e.preventDefault();



        if(password !== confirmPassword)
        {

            setMessage(
                "Passwords do not match"
            );

            return;

        }



        try
        {

            setLoading(true);



            const response =
                await resetPassword({

                    token,

                    newPassword: password

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
                "Password reset failed"
            );

        }
        finally
        {

            setLoading(false);

        }


    }





    return (

    <div className="reset-page">


        <div className="reset-card">


            <h1>
                Reset Password
            </h1>


            <p>
                Enter your new password
            </p>




            <form onSubmit={handleSubmit}>


                <label>
                    New Password
                </label>


                <PasswordInput

                    value={password}

                    setValue={setPassword}

                    placeholder="Enter new password"

                />



                <PasswordStrength

                    password={password}

                />





                <label>
                    Confirm Password
                </label>



                <PasswordInput

                    value={confirmPassword}

                    setValue={setConfirmPassword}

                    placeholder="Confirm password"

                />





                {
                    password.length > 0 &&
                    confirmPassword.length > 0 &&


                    <p
                    className={
                        password === confirmPassword
                        ?
                        "password-match success"
                        :
                        "password-match error"
                    }
                    >

                        {
                            password === confirmPassword
                            ?
                            "✓ Passwords match"
                            :
                            "✗ Passwords do not match"
                        }


                    </p>

                }







                <button

                className="reset-submit"

                disabled={loading}

                >

                    {
                        loading
                        ?
                        "Resetting..."
                        :
                        "Reset Password"
                    }


                </button>



            </form>








            {
                message &&


                <div className="reset-message">

                    {message}

                </div>

            }







            <Link to="/login">

                Back to Login

            </Link>



        </div>



    </div>

);
}
export default ResetPassword;