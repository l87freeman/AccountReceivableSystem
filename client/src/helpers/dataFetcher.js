const fetchData = async (fetchAction) => {
    try {
        const response = await fetchAction();
        return {error: null, response};
    } catch (errorResponse) {
        return {error: errorResponse.response?.data?.message || errorResponse.message, response: null};
    }
};

export default fetchData;