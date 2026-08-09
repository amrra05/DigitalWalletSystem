import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import axios from "axios";
import "./styles/VerifyEmail.css";

function VerifyEmail()
{
    const [searchParams] = useSearchParams();

    const [loading, setLoading] = useState(true);

    const [success, setSuccess] = useState(false);

    const [message, setMessage] = useState("");



    useEffect(() =>
    {
        async function verify()
        {
            const token =
                searchParams.get("token");

            if (!token)
            {
                setSuccess(false);

                setMessage("Verification token is missing.");

                setLoading(false);

                return;
            }

            try
            {
                const response =
                    await axios.get(
                        `https://localhost:7064/api/Auth/verify-email?token=${token}`
                    );

                setSuccess(true);

                setMessage(response.data.message);
            }
            catch (error)
            {
                console.log(error.response);
                setSuccess(false);

                setMessage(
                    error.response?.data?.message ||
                    "Verification failed."
                );
            }
            finally
            {
                setLoading(false);
            }
        }

        verify();

    }, [searchParams]);



    return (

        <div className="verify-page">

            <div className="verify-card">

                {
                    loading
                    ?

                    <>

                        <h2>
                            Verifying Email...
                        </h2>

                        <p>
                            Please wait...
                        </p>

                    </>

                    :

                    <>

                        <h2>

                            {
                                success
                                ?
                                "✅ Email Verified"
                                :
                                "❌ Verification Failed"
                            }

                        </h2>



                        <p>

                            {message}

                        </p>



                        <Link
                            className="login-btn"
                            to="/login"
                        >

                            Go to Login

                        </Link>

                    </>

                }

            </div>

        </div>

    );

}

export default VerifyEmail;