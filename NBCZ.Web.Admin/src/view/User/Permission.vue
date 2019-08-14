<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :label-width="100">
      <Row>
        <Col span="24">
          <div style="height: 500px;overflow: auto;">
            <function-tree-check ref="functionTreeCheck" :parent="this"></function-tree-check>
          </div>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;padding-top:10px;">
            <Button @click="parent.modelPermission=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save()">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>
  </div>
</template>
<script>
import FunctionTreeCheck from "@/view/Permission/FunctionTreeCheck";
import { getFunctions, saveFunctions } from "@/api/pubUser";
export default {
  props: { editRow: Object, parent: Object },
  components: { FunctionTreeCheck },
  data() {
    return {
      Row: {},
      modelDept: false
    };
  },
  methods: {
    getFunctions() {
      getFunctions(this.Row.userCode)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$refs.functionTreeCheck.functionSelect(data);
          } else {
            this.$Message.error({
              content: msg,
              duration: 10,
              closable: true
            });
          }
        })
        .catch(err => {});
    },
    save() {
      this.saveFunctions();
    },
    saveFunctions() {
      let checkedNodes = this.$refs.functionTreeCheck.functionTreeItems;
      let checkeds = checkedNodes.filter(p=>p.checked==true).map(p=>p.value);
      saveFunctions(this.Row.userCode, checkeds)
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          const code = resData.code;
          const msg = resData.msg;
          if (code == 1) {
            this.$Message.info("保存成功");
            this.parent.modelPermission = false;
            this.parent.setPageData();
          } else {
            this.$Message.error({
              content: msg,
              duration: 10,
              closable: true
            });
          }
        })
        .catch(err => {});
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.getFunctions();
    }
  },
  mounted() {
    // this.getFunctions();
  }
};
</script>

