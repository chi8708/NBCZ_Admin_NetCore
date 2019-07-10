import axios from '@/libs/api.request'
import store from '@/store'

var token=store.state.user.token;
export const getPage = ({pageNum ,  pageSize ,  field ,  order  }) => {
  const data = {
    pageNum:pageNum,
    pageSize:pageSize,
    field:field,
    order:order
  }
  return axios.request({
    url: 'api/PubUser/GetPage',
    headers: {Authorization:"Bearer "+token},
    data,
    method: 'post'
  })
}