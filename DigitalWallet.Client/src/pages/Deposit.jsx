import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { depositMoney } from "../services/walletService";
import "./styles/Deposit.css";


function Deposit()
{

    const navigate = useNavigate();


    const [amount,setAmount] = useState("");

    const [message,setMessage] = useState("");

    const [error,setError] = useState("");

    const [loading,setLoading] = useState(false);



    async function handleSubmit(e)
    {
        e.preventDefault();


        setError("");
        setMessage("");
        setLoading(true);


        try
        {

            await depositMoney({

                amount:Number(amount)

            });



            setMessage(
                "Deposit completed successfully"
            );


            setTimeout(() =>
            {

                navigate("/dashboard");

            },1500);


        }
        catch(error)
        {

            setError(
                "Deposit failed"
            );

        }
        finally
        {

            setLoading(false);

        }

    }



    return (

        <div className="deposit-page">


            <div className="deposit-card">


                <h1>
                    Deposit Money
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
                                "Deposit"
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


export default Deposit;