import { useEffect, useState, useMemo } from "react";
import { useLocation, useParams, Link } from "react-router-dom";
import { getPost, getPostsBySlug } from "../../Services/BlogRepository";
import { isEmptyOrSpaces } from "../../Utils/Utils";
import axios from "axios";
import { format } from "date-fns";

function PostDetail() {
    // hien thi comments
    const [comments, setComments] = useState([]);
    const [postList, setPostList] = useState();
    const [commentInput, setComment] = useState({
        comment: "",
    });

    const { search } = useLocation();
    const { year, month, day, slug: postSlug } = useParams();

    function useQuery() {
        return useMemo(() => new URLSearchParams(search), [search]);
    }
    const submitComment = (e) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append("comment", commentInput.comment);

        axios.post(`api_url`, formData).then((res) => {
            if (res.data.status === 200) {
                window.alert("Bình luận thành công.");
                setComment({
                    ...commentInput,
                    comment: "",
                });
                // Reload the page
                window.location.reload();
            } else if (res.data.status === 401) {
                window.alert("Lỗi rồi bạn ơi!");
            }
        });
    };

    const query = useQuery(),
        slug = query.get("slug"),
        p = query.get("p") ?? 1,
        ps = query.get("ps") ?? 2;

    useEffect(() => {
        document.title = "Trang  chủ";

        if (postSlug) {
            getPost(Number(year), Number(month), Number(day), postSlug).then(
                (data) => {
                    if (data) {
                        let imageUrl = isEmptyOrSpaces(data.items[0].imageUrl)
                            ? process.env.PUBLIC_URL + "/images/image_1.jpg"
                            : `${data.items[0].imageUrl}`;
                        data.items[0].imageUrl = imageUrl;
                        setPostList(data.items[0]);
                    } else setPostList();
                }
            );
        } else {
            getPostsBySlug(slug).then((data) => {
                if (data) {
                    let imageUrl = isEmptyOrSpaces(data.imageUrl)
                        ? process.env.PUBLIC_URL + "/images/image_1.jpg"
                        : `${data.imageUrl}`;
                    data.imageUrl = imageUrl;
                    setPostList(data);
                } else setPostList([]);
            });
        }
    }, [slug, year, month, day, postSlug]);

    if (postList)
        return (
            <>
                <div className="p-4 mb-5">
                    <h3 className="text-success mb-2">Tên bài viết: {postList.title}</h3>
                    <p>
                        Được tạo lúc:
                        <small className="ms-3 text-primary">
                            {format(new Date(postList.postedDate), "dd/MM/yyyy")}
                        </small>
                    </p>
                    <p className="text-secondary">
                        Tác giả:{" "}
                        <span className="text-primary">{postList.author.fullName}</span>
                    </p>
                    <p className="text-secondary">
                        Chủ đề:{" "}
                        <span className="text-primary">{postList.category.name}</span>
                    </p>
                    <img
                        width={200}
                        height={200}
                        alt={postList.title}
                        src={postList.imageUrl}
                    />
                    <h4 className="mt-3">Giới thiệu bài viết:</h4>
                    <p>{postList.shortDescription}</p>
                    <p className="text-secondary">
                        Tổng lượt xem:{" "}
                        <span className="text-primary">{postList.postCount}</span>
                    </p>
                    <figure>
                        {postList.tags &&
                            postList.tags.length > 0 &&
                            postList.tags.map((tag, i) => {
                                <h6 key={i}>
                                    <text className="text-primary">{tag.name}</text>
                                </h6>;
                            })}
                    </figure>
                </div>
                {/* Comment */}
                <div className="comment-area mt-4">
                    <div className="card card-body">
                        <h5 className="card-title">Bình luận</h5>
                        <form onSubmit={submitComment} encType="multipart/form-data">
                            <textarea
                                name="comment"
                                className="form-control"
                                rows="3"
                            ></textarea>
                            <button type="submit" className="btn btn-primary px-4 mt-2">
                                Bình luận
                            </button>
                        </form>
                    </div>
                    {/* All Comments */}
                    <div className="card card-body shadow-sm mt-3">
                        <div className="detail-area">
                            <h6 className="user-name mb-1">
                                Đoàn Quang Huy
                                <small className="ms-3 text-primary">
                                    Vào lúc: 22:00 7/4/2023
                                </small>
                            </h6>
                            <p className="user-comment mb-1">
                                Đây là bình luận ngẫu nhiên....
                            </p>
                            <div>
                                <Link to={``} className="btn btn-primary btn-sm me-2">
                                    Sửa
                                </Link>
                                <Link to={``} className="btn btn-danger btn-sm me-2">
                                    Xóa
                                </Link>
                            </div>
                        </div>
                    </div>
                    {/* end Comments */}
                </div>
            </>
        );
    else
        return (
            <>
                <p> Không tìm thấy bài viết</p>
            </>
        );
}

export default PostDetail;
