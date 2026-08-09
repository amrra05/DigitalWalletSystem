import { useState } from "react";
import { transferMoney } from "../services/walletService";
import { useNavigate } from "react-router-dom";
import "./styles/SendMoney.css";


function SendMoney()
{

    const [receiverEmail,setReceiverEmail] = useState("");

    const [amount,setAmount] = useState("");

    const [message,setMessage] = useState("");

    const [error,setError] = useState("");

    const [loading,setLoading] = useState(false);

    const navigate = useNavigate();



    async function handleSubmit(e)
    {

        e.preventDefault();


        setMessage("");

        setError("");

        setLoading(true);



        try
        {

            await transferMoney({

                receiverEmail,

                amount:Number(amount)

            });



            setMessage(
                "Money sent successfully"
            );

            setTimeout(() =>
            {
                navigate("/dashboard");

            }, 1500);
            

        }
        catch(error)
        {

            console.log(error);


            setError(
                "Transfer failed"
            );

        }
        finally
        {

            setLoading(false);

        }

    }



    return (

        <div className="send-page">


            <div className="send-card">


                <h1>
                    Send Money
                </h1>



                <form onSubmit={handleSubmit}>


                    <label>
                        Receiver Email
                    </label>


                    <input

                        type="email"

                        value={receiverEmail}

                        onChange={
                            e =>
                            setReceiverEmail(
                                e.target.value
                            )
                        }

                        placeholder="receiver@email.com"

                    />



                    <label>
                        Amount
                    </label>


                    <input

                        type="number"

                        value={amount}

                        onChange={
                            e =>
                            setAmount(
                                e.target.value
                            )
                        }

                        placeholder="Amount in JOD"

                    />



                    {
                        message &&
                        <p className="success">
                            {message}
                        </p>
                    }



                    {
                        error &&
                        <p className="error">
                            {error}
                        </p>
                    }



                    <div className="send-buttons">

                        <button 
                            disabled={loading}
                            type="submit"
                        >

                            {
                                loading
                                ?
                                "Sending..."
                                :
                                "Send Money"
                            }

                        </button>



                        <button

                            type="button"

                            className="cancel-btn"

                            onClick={() => navigate("/dashboard")}

                        >

                            Cancel

                        </button>


                    </div>


                </form>


            </div>


        </div>

    );

}


export default SendMoney;