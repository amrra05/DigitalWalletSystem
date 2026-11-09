import { useEffect, useState } from "react";

import { getAdminAuditLogs } from "../services/adminService";

import Skeleton from "../components/skeleton/Skeleton";

import BackButton from "../components/BackButton";

import "./styles/AdminAuditLogs.css";


function AdminAuditLogs()
{

    const [logs, setLogs] = useState([]);

    const [loading, setLoading] = useState(true);


    const [page, setPage] = useState(1);

    const [totalPages, setTotalPages] = useState(1);


    const [search, setSearch] = useState("");

    const [sortOrder, setSortOrder] = useState("desc");



    async function loadLogs()
    {

        try
        {

            setLoading(true);


            const data =
                await getAdminAuditLogs({

                    page,

                    pageSize: 10,

                    search,

                    sortOrder

                });


            setLogs(data.items);

            setTotalPages(
                data.totalPages
            );

        }
        catch(error)
        {

            console.log(error);

        }
        finally
        {

            setLoading(false);

        }

    }



    useEffect(() =>
    {

        loadLogs();

    }, [page, search, sortOrder]);



    return (

        <div className="admin-audit-page">


            <h1>
                Audit Logs
            </h1>



            <BackButton />



            {/* =========================
                Toolbar
            ========================= */}

            <div className="audit-toolbar">


                <input

                    className="audit-search"

                    type="text"

                    placeholder="Search ID, User ID, action, description..."

                    value={search}

                    onChange={(e) =>
                    {

                        setPage(1);

                        setSearch(
                            e.target.value
                        );

                    }}

                />



                <select

                    className="audit-filter"

                    value={sortOrder}

                    onChange={(e) =>
                    {

                        setPage(1);

                        setSortOrder(
                            e.target.value
                        );

                    }}

                >

                    <option value="desc">
                        Newest First
                    </option>

                    <option value="asc">
                        Oldest First
                    </option>

                </select>


            </div>



            {/* =========================
                Loading
            ========================= */}

            {
                loading ?

                <div className="audit-skeletons">

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                    <Skeleton height="60px" />

                </div>

                :


                /* =========================
                   Table
                ========================= */

                <table className="audit-table">


                    <thead>

                        <tr>

                            <th>
                                ID
                            </th>


                            <th>
                                User ID
                            </th>


                            <th>
                                Action
                            </th>


                            <th>
                                Description
                            </th>


                            <th>
                                Date
                            </th>

                        </tr>

                    </thead>



                    <tbody>


                    {
                        logs.length === 0 ?

                        <tr>

                            <td
                                colSpan="5"
                                className="no-logs"
                            >

                                No audit logs found.

                            </td>

                        </tr>

                        :


                        logs.map(log => (

                            <tr
                                key={log.id}
                            >


                                <td>
                                    {log.id}
                                </td>



                                <td>
                                    {log.userId}
                                </td>



                                <td>

                                    <span
                                        className="audit-action"
                                    >

                                        {log.action}

                                    </span>

                                </td>



                                <td>
                                    {log.description}
                                </td>



                                <td>

                                    {
                                        new Date(
                                            log.createdAt
                                        ).toLocaleString()
                                    }

                                </td>


                            </tr>

                        ))

                    }


                    </tbody>


                </table>

            }



            {/* =========================
                Pagination
            ========================= */}

            <div className="audit-pagination">


                <button

                    disabled={
                        page === 1
                    }

                    onClick={() =>
                        setPage(page - 1)
                    }

                >

                    Previous

                </button>



                <span>

                    Page {page} of {totalPages}

                </span>



                <button

                    disabled={
                        page === totalPages
                    }

                    onClick={() =>
                        setPage(page + 1)
                    }

                >

                    Next

                </button>


            </div>


        </div>

    );

}


export default AdminAuditLogs;