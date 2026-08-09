import "./styles/Dashboard.css";

import useAuth from "../hooks/useAuth";

import Navbar from "../components/dashboard/Navbar";
import BalanceCard from "../components/dashboard/BalanceCard";
import ProfileCard from "../components/dashboard/ProfileCard";
import QuickActions from "../components/dashboard/QuickActions";
import TransactionTable from "../components/dashboard/TransactionTable";
import useWallet from "../hooks/useWallet";
import useTransactions from "../hooks/useTransactions";



function Dashboard()
{

    const { user } = useAuth();
    const { wallet } = useWallet();
    const { transactions } = useTransactions();



    return (

        <div className="dashboard-page">


            <Navbar user={user} />



            <main className="dashboard-content">


                <div className="top-cards">


                    <BalanceCard wallet={wallet} />


                    <ProfileCard user={user} />


                </div>



                <QuickActions />



                <TransactionTable transactions={transactions} />



            </main>


        </div>

    );

}


export default Dashboard;