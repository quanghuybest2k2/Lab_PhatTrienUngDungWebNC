import axios from "axios";

export async function getPosts(
    keyword = "",
    pageSize = 10,
    pageNumber = 1,
    sortColumn = "",
    sortOrder = ""
) {
    try {
        // const response = await axios.get(`https://localhost:44309/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        const response = await axios.get(
            `https://localhost:44309/api/posts?keyword=${keyword}&PublishedOnly=true&UnPublished=false&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
        );
        //https://localhost:44309/api/posts?PublishedOnly=true&UnPublished=false&PageSize=10&PageNumber=1
        const data = response.data;
        if (data.isSuccess) {
            return data.result;
        } else {
            return null;
        }
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}
export async function getPostsBySlug(slug = "") {
    try {
        const response = await axios.get(
            `https://localhost:44309/api/posts/byslug/${slug}?PageSize=10&PageNumber=1`
        );
        const data = response.data;
        if (data.isSuccess) return data.result;
        else return null;
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}
export async function getPost(year, month, day, slug = "") {
    try {
        const response = await axios.get(
            `https://localhost:44309/api/posts?PageSize=1&PageNumber=1&PublishedOnly=true&UnPublished=false&Year=${year}&Month=${month}&Day=${day}&PostSlug=${slug}`
        );
        const data = response.data;
        if (data.isSuccess) return data.result;
        else return null;
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}
