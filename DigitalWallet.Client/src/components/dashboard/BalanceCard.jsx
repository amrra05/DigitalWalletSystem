import "./DashboardComponents.css";


function BalanceCard({ wallet })
{

    return (

        <div className="card balance-card">

            <h2>
                Current Balance
            </h2>


                <h1>

                {
                    wallet
                    ?
                    `${wallet.currency} ${wallet.balance}`
                    :
                    "Loading..."

                }

                </h1>

        </div>

    );

}


export default BalanceCard;