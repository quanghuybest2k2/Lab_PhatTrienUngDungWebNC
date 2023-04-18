import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import { Link } from 'react-router-dom';

const CommentFilterPane = () => {
    return (
        <Form
            method="get"
            className="row gy-2 gx-3 align-items-center p-2"
        >
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Keyword </Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Nhập từ khóa..."
                    name="keyword"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Tên người bình luận </Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Nhập tên người bình luận..."
                    name="userName"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Tên bài viết </Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Nhập tên bài viết..."
                    name="postTitle"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Year </Form.Label>
                <Form.Control
                    type="number"
                    placeholder="Nhập năm..."
                    name="year"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Month </Form.Label>
                <Form.Select
                    name="month"
                    title="Month"
                >
                    <option value="">-- Chọn tháng --</option>

                </Form.Select>
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden"> Day </Form.Label>
                <Form.Control
                    type="number"
                    placeholder="Nhập ngày..."
                    name="day"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="form-check-label"> Đã kiểm duyệt </Form.Label>
                <Form.Control
                    className="form-check-input"
                    type="checkbox"
                    name="censored"
                    title="Censored"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Button variant="danger" type="reset">
                    Xóa lọc
                </Button>
                <Link to="/admin/comments/edit" className="btn btn-success ms-2">
                    Thêm mới
                </Link>
            </Form.Group>
        </Form>
    );
};

export default CommentFilterPane;
