import { useEffect, useState } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import {
    Link,
    Navigate,
    useLocation,
    useNavigate,
    useParams,
} from "react-router-dom";
import { addOrUpdateTag, getTagById } from "../../../Services/tagRepository";
import { isInteger, decode } from "../../../Utils/Utils";

const Edit = () => {
    const initialState = {
        id: 0,
        name: "",
        urlSlug: "",
        description: "",
        postCount: 0,
    };

    const [validated, setValidated] = useState(false);
    const [tag, setTag] = useState(initialState);

    let { id } = useParams();
    id = id ?? 0;

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        document.title = "Thêm/cập nhật thẻ";
        getTagById(id).then((data) => {
            if (data) setTag(data);
            else setTag(initialState);
        });
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (e.currentTarget.checkValidity() === false) {
            e.stopPropagation();
            setValidated(true);
        } else {
            let form = new FormData(e.target);
            // const data = Object.fromEntries(form.entries());
            addOrUpdateTag(form).then((data) => {
                if (data) {
                    alert("Đã lưu thành công!");
                    const { from } = location.state || {};
                    navigate(from?.pathname || "/admin/tags", { replace: true });
                } else alert("Đã xảy ra lỗi!");
            });
        }
    };

    if (id && !isInteger(id))
        return <Navigate to={`/400?redirectTo=/admin/tags`} />;
    return (
        <>
            <Form
                method="post"
                encType="multipart/form-data"
                onSubmit={handleSubmit}
                noValidate
                validated={validated}
            >
                <Form.Control type="hidden" name="id" value={tag.id} />
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label"> Tên thẻ </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            name="name"
                            title="Name"
                            required
                            value={tag.name || ""}
                            onChange={(e) => setTag({ ...tag, name: e.target.value })}
                        />
                        <Form.Control.Feedback type="invalid">
                            Không được bỏ trống.
                        </Form.Control.Feedback>
                    </div>
                </div>
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label"> Slug </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            name="urlSlug"
                            title="Url slug"
                            value={tag.urlSlug || ""}
                            onChange={(e) => setTag({ ...tag, urlSlug: e.target.value })}
                        />
                    </div>
                </div>
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label"> Mô tả </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            as="textarea"
                            rows={10}
                            required
                            name="description"
                            title="Description"
                            value={decode(tag.description || "")}
                            onChange={(e) => setTag({ ...tag, description: e.target.value })}
                        />
                        <Form.Control.Feedback type="invalid">
                            Không được bỏ trống.
                        </Form.Control.Feedback>
                    </div>
                </div>
                <div className="text-center">
                    <Button variant="primary" type="submit">
                        Lưu các thay đổi
                    </Button>
                    <Link to="/admin/tags" className="btn btn-danger ms-2">
                        Hủy và quay lại
                    </Link>
                </div>
            </Form>
        </>
    );
};

export default Edit;
