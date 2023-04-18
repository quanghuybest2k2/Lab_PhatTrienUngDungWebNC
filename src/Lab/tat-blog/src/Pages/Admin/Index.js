import { useEffect, useState } from "react";
import {
    getTotalAuthor,
    getTotalCategories,
    getTotalNewestSubscriber,
    getTotalPosts,
    getTotalSubscribers,
    getTotalUnpublishedPosts,
    getTotalWaitingComments,
} from "../../Services/dashboardRepository";

const Index = () => {
    const [getTotal, setTotal] = useState({
        totalPosts: 0,
        totalUnpublishedPosts: 0,
        totalCategories: 0,
        totalAuthor: 0,
        totalWaitingComments: 0,
        totalSubscribers: 0,
        totalNewestSubscriber: 0,
    });

    useEffect(() => {
        Promise.all([
            getTotalAuthor(),
            getTotalCategories(),
            getTotalNewestSubscriber(),
            getTotalPosts(),
            getTotalUnpublishedPosts(),
            getTotalWaitingComments(),
            getTotalSubscribers(),
        ]).then(
            ([
                totalAuthor,
                totalCategories,
                totalNewestSubscriber,
                totalPosts,
                totalSubscribers,
                totalUnpublishedPosts,
                totalWaitingComments,
            ]) => {
                setTotal({
                    totalAuthor: totalAuthor ?? 0,
                    totalCategories: totalCategories ?? 0,
                    totalNewestSubscriber: totalNewestSubscriber ?? 0,
                    totalPosts: totalPosts ?? 0,
                    totalSubscribers: totalSubscribers ?? 0,
                    totalUnpublishedPosts: totalUnpublishedPosts ?? 0,
                    totalWaitingComments: totalWaitingComments ?? 0,
                });
            }
        );
    }, []);

    return (
        <div class="container-fluid">
            {/* <!-- Content Row --> */}
            <div class="row">
                {/* <!-- Earnings (Monthly) Card Example --> */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-primary shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                        Tổng số bài viết
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalPosts}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-clipboard fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* <!-- Earnings (Monthly) Card Example --> */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-success shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                        Số bài viết chưa xuất bản
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalUnpublishedPosts}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-check-square fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* <!-- Earnings (Monthly) Card Example --> */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-warning shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        Số lượng chủ đề
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalCategories}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-eye fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {/* <!-- Pending Requests Card Example --> */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-warning shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        Số lượng tác giả
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalAuthor}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-id-card fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {/* @*phe duyet*@ */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-warning shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        Số lượng bình luận đang chờ phê duyệt
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalWaitingComments}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-comments fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {/* @*nguoi theo doi*@ */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-warning shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        Số lượng người theo dõi
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalSubscribers}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-file-video-o fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {/* @*dang ky theo doi*@ */}
                <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-warning shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        Số lượng người mới theo dõi đăng ký (lấy số liệu trong
                                        ngày).
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        {getTotal.totalNewestSubscriber}
                                    </div>
                                </div>
                                <div class="col-auto">
                                    <i class="fa fa-plus-circle fs-2" aria-hidden="true"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};
export default Index;
