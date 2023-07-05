export const formatDate = (dateString) => {
    if(!dateString){
        return "";
    }
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const formattedDate = `${year}-${month}-${day}`;
    return formattedDate;
  };

export const dateDiffSeconds = (date1, date2) => {
    const diffInMs = Math.abs(new Date(date2) - new Date(date1));
    const diff = Math.floor(diffInMs / 1000);
    
    return diff;
}