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
import {
    addOrUpdateAuthor,
    getAuthorById,
} from "../../../Services/authorRepository";
import { isInteger, decode, isEmptyOrSpaces } from "../../../Utils/Utils";

const Edit = () => {
    const initialState = {
        id: 0,
        fullName: "",
        urlSlug: "",
        imageUrl: "",
        email: "",
        notes: "",
        postCount: 0,
    };

    const [validated, setValidated] = useState(false);
    const [author, setAuthor] = useState(initialState);

    let { id } = useParams();
    id = id ?? 0;

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        document.title = "Thêm/cập nhật tác giả";
        getAuthorById(id).then((data) => {
            if (data) setAuthor(data);
            else setAuthor(initialState);
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
            addOrUpdateAuthor(form).then((data) => {
                if (data) {
                    alert("Đã lưu thành công!");
                    const { from } = location.state || {};
                    navigate(from?.pathname || "/admin/categories", { replace: true });
                } else alert("Đã xảy ra lỗi!");
            });
        }
    };

    if (id && !isInteger(id))
        return <Navigate to={`/400?redirectTo=/admin/authors`} />;
    return (
        <>
            <Form
                method="post"
                encType="multipart/form-data"
                onSubmit={handleSubmit}
                noValidate
                validated={validated}
            >
                <Form.Control type="hidden" name="id" value={author.id} />
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label">

                        Tên tác giả
                    </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            name="fullName"
                            title="Name"
                            required
                            value={author.fullName || ""}
                            onChange={(e) =>
                                setAuthor({ ...author, fullName: e.target.value })
                            }
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
                            value={author.urlSlug || ""}
                            onChange={(e) =>
                                setAuthor({ ...author, urlSlug: e.target.value })
                            }
                        />
                    </div>
                </div>
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label"> Email </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            name="email"
                            title="Email"
                            required
                            value={author.email || ""}
                            onChange={(e) => setAuthor({ ...author, email: e.target.value })}
                        />
                    </div>
                </div>
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label"> Ghi chú </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="text"
                            as="textarea"
                            rows={10}
                            required
                            name="notes"
                            title="Description"
                            value={decode(author.notes || "")}
                            onChange={(e) => setAuthor({ ...author, notes: e.target.value })}
                        />
                        <Form.Control.Feedback type="invalid">
                            Không được bỏ trống.
                        </Form.Control.Feedback>
                    </div>
                </div>
                {!isEmptyOrSpaces(author.imageUrl) && (
                    <div className="row mb-3">
                        <Form.Label className="col-sm-2 col-form-label">

                            Hình hiện tại
                        </Form.Label>
                        <div className="col-sm-10">
                            <img src={author.imageUrl} alt={author.fullName} />
                        </div>
                    </div>
                )}
                <div className="row mb-3">
                    <Form.Label className="col-sm-2 col-form-label">

                        Chọn hình ảnh
                    </Form.Label>
                    <div className="col-sm-10">
                        <Form.Control
                            type="file"
                            name="imageFile"
                            accept="image/*"
                            title="Image file"
                            onChange={(e) =>
                                setAuthor({ ...author, imageFile: e.target.files[0] })
                            }
                        />
                    </div>
                </div>
                <div className="text-center">
                    <Button variant="primary" type="submit">
                        Lưu các thay đổi
                    </Button>
                    <Link to="/admin/authors" className="btn btn-danger ms-2">
                        Hủy và quay lại
                    </Link>
                </div>
            </Form>
        </>
    );
};

export default Edit;
