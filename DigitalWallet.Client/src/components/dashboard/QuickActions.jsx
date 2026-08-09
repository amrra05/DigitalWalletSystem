import "./DashboardComponents.css";
import { useNavigate } from "react-router-dom";


function QuickActions()
{
    const navigate = useNavigate();

    return (

        <div className="quick-actions">


                <button
                onClick={() => navigate("/send-money")}
                >
                Send Money
                </button>


                <button
                    onClick={() => navigate("/deposit")}
                >
                    Deposit
                </button>


                <button
                    onClick={() => navigate("/withdraw")}
                >
                    Withdraw
                </button>


                <button
                    onClick={() => navigate("/transactions")}
                >
                    History
                </button>


                <button onClick={() => navigate("/settings")}>
                    Settings
                </button>


        </div>

    );

}


export default QuickActions;