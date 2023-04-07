import axios from 'axios';

export async function getComment(postId, pageSize = 10, pageNumber = 1) {
    try {
        const response = await axios.get(
            `https://localhost:7029/api/posts/${postId}/comments?PageSize=${pageSize}&PageNumber=${pageNumber}`
        );
        const data = response.data;
        if (data.isSuccess) return data.result;
        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getCommentForPost(postId, userName, content) {
    try {
        const response = await axios.post(`https://localhost:7029/api/comments`, {
            userName: userName,
            content: content,
            postID: postId,
            censored: true,
            postDate: new Date().toISOString()
        });
        const data = response.data;
        if (data.isSuccess) return data.result;
        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}
