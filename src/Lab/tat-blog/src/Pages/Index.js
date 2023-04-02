import React, { useEffect, useState } from "react";
import PostItem from "../Components/PostItem";
import { getPosts } from "../Services/BlogRepository";

const Index = () => {
    const [postList, setPostList] = useState([]);

    useEffect(() => {
        document.title = "Trang chủ";
        getPosts().then((data) => {
            if (data) {
                setPostList(data.items);
            } else {
                setPostList([]);
            }
        });
    }, []);

    if (postList.length > 0) {
        return (
            <div className="p-4">
                {postList.map((item) => {
                    return <PostItem postItem={item} />;
                })}
                ;
            </div>
        );
    } else {
        return (
            <>
                <p>Không có bài viết nào!</p>
            </>
        );
    }
};

export default Index;
