import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { withdrawMoney } from "../services/walletService";
import "./styles/Withdraw.css";


function Withdraw()
{

    const navigate = useNavigate();


    const [amount,setAmount] = useState("");

    const [message,setMessage] = useState("");

    const [error,setError] = useState("");

    const [loading,setLoading] = useState(false);



    async function handleSubmit(e)
    {

        e.preventDefault();


        setMessage("");

        setError("");

        setLoading(true);



        try
        {

            await withdrawMoney({

                amount:Number(amount)

            });



            setMessage(
                "Withdrawal completed successfully"
            );



            setTimeout(() =>
            {

                navigate("/dashboard");

            },1500);



        }
        catch(error)
        {

            setError(
                "Withdrawal failed. Check your balance."
            );

        }
        finally
        {

            setLoading(false);

        }

    }



    return (

        <div className="withdraw-page">


            <div className="withdraw-card">


                <h1>
                    Withdraw Money
                </h1>



                <form onSubmit={handleSubmit}>


                    <label>
                        Amount
                    </label>


                    <input

                        type="number"

                        placeholder="Enter amount"

                        value={amount}

                        onChange={
                            e =>
                            setAmount(e.target.value)
                        }

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




                    <div className="buttons">


                        <button
                            disabled={loading}
                        >

                            {
                                loading
                                ?
                                "Processing..."
                                :
                                "Withdraw"
                            }

                        </button>




                        <button

                            type="button"

                            className="cancel"

                            onClick={
                                () =>
                                navigate("/dashboard")
                            }

                        >

                            Cancel

                        </button>


                    </div>


                </form>


            </div>


        </div>

    );

}


export default Withdraw;