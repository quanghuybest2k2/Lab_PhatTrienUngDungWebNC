import axios from 'axios'

export async function getPosts(keyword = '', PublishedOnly = true, UnPublished = false, pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        // const response = await axios.get(`https://localhost:44309/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        const response = await axios.get(`https://localhost:44309/api/posts?keyword=${keyword}&PublishedOnly=${PublishedOnly}&UnPublished=${UnPublished}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        //https://localhost:44309/api/posts?PublishedOnly=true&UnPublished=false&PageSize=10&PageNumber=1
        const data = response.data;
        if (data.isSuccess) {
            return data.result;
        } else {
            return null;
        }
    }
    catch (error) {
        console.log('Error', error.message);
        return null;
    }
}
