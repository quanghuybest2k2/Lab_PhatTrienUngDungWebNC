import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getCategories } from "../Services/Widget";

const CategoriesWidget = () => {
    const [categoryList, setcategoryList] = useState([]);

    useEffect(() => {
        getCategories().then((data) => {
            if (data) {
                setcategoryList(data.items);
            } else {
                setcategoryList([]);
            }
        });
    }, []);

    return (
        <div className="mb-4">
            <h3 className="text-success mb-2">Các chủ đề</h3>
            {categoryList.length > 0 && (
                <ListGroup>
                    {categoryList.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                {/* https://localhost:44309/api/categories?ShowOnMenu=true&PageSize=10&PageNumber=1 */}
                                <Link
                                    to={`/blog/category?slug=${item.urlSlug}`}
                                    title={item.description}
                                    key={index}
                                >
                                    {item.name}
                                    <span>&nbsp;({item.postCount})</span>
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            )}
        </div>
    );
};

export default CategoriesWidget;
