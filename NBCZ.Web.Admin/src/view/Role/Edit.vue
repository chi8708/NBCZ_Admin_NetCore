<template>
  <div>
    <Form ref="formInline" label-position="right" :model="Row" :rules="rule" :label-width="100">
      <Row>
        <Col span="24">
          <FormItem label="角色名" prop="roleName">
            <Input v-model="Row.roleName" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <FormItem label="备注">
            <Input v-model="Row.remark" />
          </FormItem>
        </Col>
      </Row>
      <Row>
        <Col span="24">
          <div style="text-align:center;">
            <Button @click="parent.modelEdit=false">取消</Button>
            <Button style="margin-left:20px;" type="primary" @click="save('formInline')">保存</Button>
          </div>
        </Col>
      </Row>
    </Form>
  </div>
</template>
<script>
import { add , edit } from "@/api/pubRole";
export default {
  props: { editRow: Object, parent: Object },
  computed: {},
  data() {
    return {
      Row: {},
      Roles: [],
      Depts: [],
      DeptTree: [],
      DeptTreeItems: [],
      modelDept: false,
      rule: {
        roleName: [
          {
            required: true,
            message: "角色名必填",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    save() {
      if (this.parent.isAdd) {
        this.saveAdd();
      } else {
        this.saveEdit();
      }
    },
    saveAdd() {
      this.saveValidate().then(r => {
        if (r) {
          add(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("添加成功");
                this.parent.modelEdit = false;
                this.parent.setPageData(1);
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
      });
    },
    saveEdit() {
      this.saveValidate().then(r => {
        if (r) {
          edit(this.Row)
            .then(res => {
              const resData = res.data;
              const data = resData.data;
              const code = resData.code;
              const msg = resData.msg;
              if (code == 1) {
                this.$Message.info("编辑成功");
                this.parent.modelEdit = false;
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
      });
    },
    saveValidate(name = "formInline") {
      return this.$refs[name].validate(valid => {
        if (!valid) {
          this.$Message.warning("请检查表单数据！");
          return false;
        } else {
          return true;
        }
      });
    },
    handleReset(name = "formInline") {
      this.$refs[name].resetFields();
    }
  },
  watch: {
    editRow(newVal, oldVal) {
      this.Row = Object.assign({}, newVal);
      this.Row.sex = this.Row.sex === false ? 0 : 1;
    }
  },
  mounted() {
  }
};
</script>

