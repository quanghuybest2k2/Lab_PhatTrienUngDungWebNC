import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { Link } from 'react-router-dom';
import Loading from '../../../Components/Loading';
import { getComments } from '../../../Services/commentRepository';
import CommentFilterPane from '../../../Components/Admin/CommentFilterPane';

const Comments = () => {
    const [commentList, setCommentList] = useState([]);
    const [commentQuery, setCommentQuery] = useState({});
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách các bình luận';
        getComments().then((data) => {
            if (data) {
                setCommentQuery((pre) => {
                    return { ...pre, to: '/admin/comments' };
                });
                setCommentList(data.items);
            } else setCommentList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách các bình luận</h1>
            <CommentFilterPane />
            {isVisibleLoading ? (
                <Loading />
            ) : (
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tên người bình luận</th>
                            <th>Nội dung</th>
                            <th>Kiểm duyệt</th>
                        </tr>
                    </thead>
                    <tbody>
                        {commentList && commentList.length > 0 ? (
                            commentList.map((item, index) => (
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/comments/edit/${item.id}`} className="text-bold">
                                            {item.userName}
                                        </Link>
                                        <p className="text-muted">Ngày bình luận: {new Date(item.postDate).toISOString().replace("T", " ").substring(0, 19)}</p>
                                    </td>
                                    <td>{item.content}</td>
                                    <td>
                                        {item.censored ? (
                                            <Button variant="success">
                                                Có
                                            </Button>
                                        ) : (
                                            <Button variant="warning">
                                                Không
                                            </Button>
                                        )}
                                    </td>
                                    <td>
                                        <Button variant="danger">
                                            Xoá
                                        </Button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={4}>
                                    <h4 className="text-danger text-center">Không tìm thấy các bình luận nào</h4>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            )}
        </>
    );
};

export default Comments;
