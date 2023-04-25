import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import { Link } from 'react-router-dom';

const CategoryFilterPane = () => {

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
                <Form.Label className="form-check-label"> Hiển thị trên Menu </Form.Label>
                <Form.Control
                    className="form-check-input"
                    type="checkbox"
                    name="showOnMenu"
                />
            </Form.Group>
            <Form.Group className="col-auto">
                <Button variant="danger" type="reset">
                    Xóa lọc
                </Button>
                <Link to="/admin/categories/edit" className="btn btn-success ms-2">
                    Thêm mới
                </Link>
            </Form.Group>
        </Form>
    );
};

export default CategoryFilterPane;
